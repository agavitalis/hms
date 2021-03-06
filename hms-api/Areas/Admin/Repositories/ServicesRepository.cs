using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class ServicesRepository : IServices
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ITransactionLog _transaction;
        private readonly IAccount _account;

        public ServicesRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration config, ITransactionLog transaction, IAccount account)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _config = config;
            _transaction = transaction;
            _account = account;
        }


        public async Task<IEnumerable<ServiceDtoForView>> GetAllServices()
        {
            var services = await _applicationDbContext.Services.Include(s=>s.ServiceCategory).OrderBy(s => s.Name).ToListAsync();

            return _mapper.Map<IEnumerable<ServiceDtoForView>>(services);
        }

        public async Task<Service> GetServiceByIdAsync(string id)
        {
            try
            {
                var service = await _applicationDbContext.Services.FindAsync(id);

                return service;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Service>> GetServiceByCategoryAsync(string ServiceCategoryId) => await _applicationDbContext.Services.Where(s => s.ServiceCategoryId == ServiceCategoryId).OrderBy(s => s.Name).ToListAsync();
      

        public async Task<bool> CreateService(Service service)
        {
            try
            {
                if (service == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Add(service);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateService(Service serviceToEdit)
        {
            try
            {
                if (serviceToEdit == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Update(serviceToEdit);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteService(Service service)
        {
            try
            {
                if (service == null)
                {
                    return false;
                }

                _applicationDbContext.Services.Remove(service);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckIfServicesExist(List<string> serviceIds)
        {
            if (serviceIds == null)
                return false;

            var idNotInServices = serviceIds.Where(x => _applicationDbContext.Services.Any(y => y.Id == x));

            return idNotInServices.Any();
        }

        public async Task<int> GetServiceCount()
        {

            var servicesCount = await _applicationDbContext.Services.CountAsync();
            return servicesCount;
               
        }




        public async Task<bool> CreateServiceRequest(ServiceRequestDtoForCreate serviceRequest, string invoiceId)
        {
            try
            {
                
                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
                string priceCalculationFormular = "";

                



                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == serviceRequest.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                //get the drug price based on the health plan above
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();




                decimal totalServicePricing = 0;
                decimal amountDue = 0;
                decimal HMOAmount = 0;


                if (serviceRequest == null || string.IsNullOrEmpty(invoiceId))
                    return false;
                if (serviceRequest.IdType.ToLower() == "appointment")
                {
                    for (int i = 0; i < serviceRequest.ServiceId.Count; i++)
                    {
                       var service = _applicationDbContext.Services.Find(serviceRequest.ServiceId[i]);
                       
                        
                        if (HMOHealthPlanPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                     new ServiceRequest
                                     {
                                         ServiceId = serviceRequest.ServiceId[i],
                                         Amount = HMOHealthPlanServicePrice.Price,
                                         PaymentStatus = "Awaiting HMO Payment",
                                         ServiceInvoiceId = invoiceId,
                                         AppointmentId = serviceRequest.Id
                                     });

                            }
                        }
                        else if (HMOHealthPlanSubGroupPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = HMOHealthPlanServicePrice.Price,
                                        PaymentStatus = "Awaiting HMO Payment",
                                        ServiceInvoiceId = invoiceId,
                                        AppointmentId = serviceRequest.Id
                                    });
                            }
                        }

                        else if (NHISHealthPlanPatient != null)
                        {
                            var NHISDrug = await _applicationDbContext.NHISHealthPlanServices.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (NHISDrug != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = service.Cost * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100,
                                        PaymentStatus = "False",
                                        ServiceInvoiceId = invoiceId,
                                        AppointmentId = serviceRequest.Id
                                    });
                            }
                        }
                    }
                }
                else if (serviceRequest.IdType.ToLower() == "consultation")
                {
                    for (int i = 0; i < serviceRequest.ServiceId.Count; i++)
                    {
                        var service = _applicationDbContext.Services.Find(serviceRequest.ServiceId[i]);
                        if (HMOHealthPlanPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                     new ServiceRequest
                                     {
                                         ServiceId = serviceRequest.ServiceId[i],
                                         Amount = HMOHealthPlanServicePrice.Price,
                                         PaymentStatus = "Awaiting HMO Payment",
                                         ServiceInvoiceId = invoiceId,
                                         ConsultationId = serviceRequest.Id
                                     });

                            }
                        }
                        else if (HMOHealthPlanSubGroupPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = HMOHealthPlanServicePrice.Price,
                                        PaymentStatus = "Awaiting HMO Payment",
                                        ServiceInvoiceId = invoiceId,
                                        ConsultationId = serviceRequest.Id
                                    });
                            }
                        }

                        else if (NHISHealthPlanPatient != null)
                        {
                            var NHISDrug = await _applicationDbContext.NHISHealthPlanServices.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (NHISDrug != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = service.Cost * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100,
                                        PaymentStatus = "False",
                                        ServiceInvoiceId = invoiceId,
                                        ConsultationId = serviceRequest.Id
                                    });
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < serviceRequest.ServiceId.Count; i++)
                    {
                        if (HMOHealthPlanPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                              await  _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = HMOHealthPlanServicePrice.Price,
                                        PaymentStatus = "Awaiting HMO Payment",
                                        ServiceInvoiceId = invoiceId,
                                        
                                    });

                            }
                        }
                        else if (HMOHealthPlanSubGroupPatient != null)
                        {
                            var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (HMOHealthPlanServicePrice != null)
                            {
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = HMOHealthPlanServicePrice.Price,
                                        PaymentStatus = "Awaiting HMO Payment",
                                        ServiceInvoiceId = invoiceId,
                                        ConsultationId = serviceRequest.Id
                                    });
                            }
                        }

                        else if (NHISHealthPlanPatient != null)
                        {
                            var NHISService = await _applicationDbContext.NHISHealthPlanServices.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.ServiceId == serviceRequest.ServiceId[i]).FirstOrDefaultAsync();

                            if (NHISService != null)
                            {
                                var service = _applicationDbContext.Services.Find(serviceRequest.ServiceId[i]);
                                await _applicationDbContext.ServiceRequests.AddAsync(
                                    new ServiceRequest
                                    {
                                        ServiceId = serviceRequest.ServiceId[i],
                                        Amount = service.Cost * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100,
                                        PaymentStatus = "False",
                                        ServiceInvoiceId = invoiceId,
                                    });
                            }
                        }
                    }
                }
                
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      

        public async Task<string> GenerateInvoiceForServiceRequest(ServiceRequestDtoForCreate serviceRequest)
        {
            try
            {
                if (serviceRequest == null)
                    return null;
                
               
                
                List<Service> services = new List<Service>();


                //grab the patient complete details
                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == serviceRequest.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(p => p.HMOHealthPlan).ThenInclude(a => a.HMO).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).Include(p => p.HMOSubUserGroup).ThenInclude(a => a.HMOHealthPlan).ThenInclude(a => a.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();

                decimal totalServicePricing = 0;
                decimal amountDue = 0;
                decimal NHISAmount = 0;
                decimal HMOAmount = 0;

                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                string priceCalculationFormular = "";
                decimal AmountToBePaidByHMO = 0;

                foreach (var serviceId in serviceRequest.ServiceId)
                {
                    


                    var service = _applicationDbContext.Services.Find(serviceId);


                    //get the drug price based on the health plan patient belongs to above
                    if (HMOHealthPlanPatient != null)
                    {
                       var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.ServiceId == service.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanServicePrice != null)
                        {
                            priceTotal = HMOHealthPlanServicePrice.Price;
                            AmountToBePaidByPatient = 0;
                            priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                        }
                    }
                    else if (HMOHealthPlanSubGroupPatient != null)
                    {
                       var HMOHealthPlanServicePrice = await _applicationDbContext.HMOHealthPlanServicePrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.ServiceId == service.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanServicePrice != null)
                        {
                            priceTotal = HMOHealthPlanServicePrice.Price;
                            AmountToBePaidByPatient = 0;
                            priceCalculationFormular = HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlan.Name;
                        }
                    }
                    else if (NHISHealthPlanPatient != null)
                    {
                       var NHISService = await _applicationDbContext.NHISHealthPlanServices.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.ServiceId == service.Id).FirstOrDefaultAsync();
                        if (NHISService != null)
                        {

                            priceTotal = service.Cost;
                            AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                            priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                        }
                    }

                    else
                    {
                        priceTotal = service.Cost;
                        AmountToBePaidByPatient = service.Cost;
                        priceCalculationFormular = "Default Price";
                    }
                    totalServicePricing += priceTotal;

                    amountDue += AmountToBePaidByPatient;
                    HMOAmount += priceTotal;

                }

                if (HMOHealthPlanPatient != null)
                {

                    var serviceInvoice = new ServiceInvoice()
                    {
                        AmountTotal = totalServicePricing,
                        AmountToBePaidByPatient = amountDue,
                        AmountToBePaidByHMO = HMOAmount,
                        PaymentStatus = "Awaiting HMO Payment",
                        PriceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name,
                        GeneratedBy = serviceRequest.GeneratedBy,
                        PatientId = serviceRequest.PatientId
                    };

                    await _applicationDbContext.ServiceInvoices.AddAsync(serviceInvoice);
                    await _applicationDbContext.SaveChangesAsync();
                    return serviceInvoice.Id;
                }

                else
                {

                    var serviceInvoice = new ServiceInvoice()
                    {
                        AmountTotal = totalServicePricing,
                        AmountToBePaidByPatient = amountDue,
                        AmountToBePaidByHMO = HMOAmount,
                        PaymentStatus = "NOT PAID",
                        GeneratedBy = serviceRequest.GeneratedBy,
                        PatientId = serviceRequest.PatientId
                    };

                    await _applicationDbContext.ServiceInvoices.AddAsync(serviceInvoice);
                    await _applicationDbContext.SaveChangesAsync();
                    return serviceInvoice.Id;

                }   
            }
            catch (Exception ex)
            {
                return null;
            }
        }
  
        public PagedList<ServiceInvoiceDtoForView> GetServiceInvoicesPagination(PaginationParameter paginationParameter)
        {
            var invoices =  _applicationDbContext.ServiceInvoices.Include(a => a.ServiceRequests).Include(p => p.Patient).OrderByDescending(s => s.DateGenerated).ToList();

            var serviceInvoiceToReturn = _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

            return PagedList<ServiceInvoiceDtoForView>.ToPagedList(serviceInvoiceToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
            
        }

        //without pagination
        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoices()
        {
            var invoices = await _applicationDbContext.ServiceInvoices.Include(a => a.ServiceRequests).Include(p => p.Patient).OrderBy(s => s.DateGenerated).ToListAsync();

            return  _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

        }

        public async Task<IEnumerable<dynamic>> GetServiceRequestInAnInvoice(string invoiceId)
        {
            var serviceRequest = await _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == invoiceId)
                .Include(s => s.ServiceInvoice).ThenInclude(i => i.Patient)
                .Include(p => p.Service).ThenInclude(s=>s.ServiceCategory)
                .Select(p => new
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaymentStatus = p.PaymentStatus,
                    status = p.Status,
                    serviceInvoiceId = p.ServiceInvoiceId,
                    ServiceName = p.Service.Name,
                    serviceCategoryName = p.Service.ServiceCategory.Name,
                    patientFirstName = p.ServiceInvoice.Patient.FirstName,
                    patientLastName = p.ServiceInvoice.Patient.LastName,

                })
                .OrderBy(c => c.PaymentStatus)
                .ToListAsync();

            return serviceRequest;
        }

        public PagedList<ServiceInvoiceDtoForView> GetServiceInvoiceForPatient(string patientId, PaginationParameter paginationParameter)
        {
            var patientProfile =  _applicationDbContext.PatientProfiles.Where(a => a.PatientId == patientId).FirstOrDefault();
            if (patientProfile != null)
            {
                var invoices = _applicationDbContext.ServiceInvoices.Where(a => a.PatientId == patientId).Include(p => p.ServiceRequests).OrderByDescending(c => c.DateGenerated).ToList();

                var serviceToReturn = _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);

                return PagedList<ServiceInvoiceDtoForView>.ToPagedList(serviceToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
            }
            return null;
        }

        //without pagination
        public async Task<IEnumerable<ServiceInvoiceDtoForView>> GetServiceInvoiceForPatient(string patientId)
        {
            var patientProfile = await _applicationDbContext.PatientProfiles.Where(a => a.PatientId == patientId).FirstOrDefaultAsync();

            var invoices = await _applicationDbContext.ServiceInvoices.Where(a => a.PatientId == patientId).Include(p => p.ServiceRequests).OrderByDescending(c => c.DateGenerated).ToListAsync();

            return _mapper.Map<IEnumerable<ServiceInvoiceDtoForView>>(invoices);


        }


        public async Task<bool> CheckIfServiceRequestIdExist(List<string> serviceRequestId)
        {
            if (serviceRequestId == null)
                return false;

            var idNotInServiceRequests = serviceRequestId.Where(x => _applicationDbContext.ServiceRequests.Any(y => y.Id == x));
            return idNotInServiceRequests.Any();
        }


        public async Task<bool> CheckIfAmountPaidIsCorrect(ServiceRequestPaymentDto services)
        {
            if (services == null)
                return false;

            decimal amountTotal = 0;
            //check if the amount tallies
            services.ServiceRequestId.ForEach( serviceRequestId =>
            {
                var service =  _applicationDbContext.ServiceRequests.Where(i => i.Id == serviceRequestId).FirstOrDefault();
                amountTotal += service.Amount;

            });

            if(amountTotal != services.TotalAmount)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> PayForServices(ServiceRequestPaymentDto services)
        {
            int servicesPaid = 0;
            string serviceInvoiceId = "";
            string transactionType = "Credit";
            string invoiceType = "Service Request";
            DateTime transactionDate = DateTime.Now;
            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == services.PatientId).FirstOrDefaultAsync();
            services.ServiceRequestId.ForEach( serviceRequestId =>
           {
               var ServiceRequest =  _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
               ServiceRequest.PaymentStatus = "PAID";

               serviceInvoiceId = ServiceRequest.ServiceInvoiceId;
               servicesPaid++;
                _applicationDbContext.ServiceRequests.Update(ServiceRequest);
              
           });

            //log transactions
            services.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest =  _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                 _transaction.LogTransaction(ServiceRequest.Amount, transactionType, invoiceType, ServiceRequest.ServiceInvoiceId, services.PaymentMethod, transactionDate, patient.Patient.Id, services.InitiatorId);

            });

            await _applicationDbContext.SaveChangesAsync();

            //now check of all the servies in this invoice was paid for
            var serviceCount = await  _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == serviceInvoiceId).CountAsync();

            var ServiceInvoice = await _applicationDbContext.ServiceInvoices.FirstOrDefaultAsync(s => s.Id == serviceInvoiceId);

            if (serviceCount == servicesPaid)
            {
                ServiceInvoice.PaymentStatus = "PAID";
                ServiceInvoice.PaymentMethod = services.PaymentMethod;
                ServiceInvoice.DatePaid = DateTime.Now;
            }
            else
            {
                ServiceInvoice.PaymentStatus = "INCOMPLETE";
            }
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PayForServicesWithAccount(ServiceRequestPaymentDto services)
        {
            int servicesPaid = 0;
            string serviceInvoiceId = "";
            string transactionType = "Credit";
            string invoiceType = "Service Request";
            string accountTransactionType = "Debit";
            string accountInvoiceType = "Account";
            string accountInvoiceId = null;
            decimal totalAmount = 0;
            string accountPaymentMethod = null;
            DateTime transactionDate = DateTime.Now;

            var patient = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == services.PatientId).FirstOrDefaultAsync();
            
            services.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                totalAmount += ServiceRequest.Amount;               
            });
            if (patient.Account.AccountBalance < totalAmount)
            {
                return false;
            }
            services.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                ServiceRequest.PaymentStatus = "PAID";
                serviceInvoiceId = ServiceRequest.ServiceInvoiceId;
               
                 var account = _applicationDbContext.Accounts.FirstOrDefault(s => s.Id == patient.AccountId);
                      

                        var accountInvoiceToCreate = new AccountInvoice();

                        accountInvoiceToCreate = new AccountInvoice()
                        {
                            Amount = ServiceRequest.Amount,
                            GeneratedBy = services.InitiatorId,
                            PaymentMethod = services.PaymentMethod,
                            TransactionReference = services.TransactionReference,
                            AccountId = account.Id,
                        };

                     

                    _applicationDbContext.AccountInvoices.Add(accountInvoiceToCreate);
                    _applicationDbContext.SaveChanges();
                accountInvoiceId = accountInvoiceToCreate.Id;
                servicesPaid++;




            });

            var account = await _applicationDbContext.Accounts.FirstOrDefaultAsync(s => s.Id == patient.AccountId);
            account.AccountBalance -= totalAmount;
            _applicationDbContext.SaveChanges();
            
            services.ServiceRequestId.ForEach(serviceRequestId =>
            {
                var ServiceRequest = _applicationDbContext.ServiceRequests.FirstOrDefault(s => s.Id == serviceRequestId);
                _transaction.LogTransaction(ServiceRequest.Amount, transactionType, invoiceType, serviceRequestId, services.PaymentMethod, transactionDate, patient.Patient.Id, services.InitiatorId);
                _transaction.LogAccountTransaction(ServiceRequest.Amount, accountTransactionType, accountInvoiceType, accountInvoiceId, accountPaymentMethod, transactionDate, patient.Account.Id, services.InitiatorId);
            });
          

            //now check of all the servies in this invoice was paid for
            var serviceCount = await _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == serviceInvoiceId).CountAsync();

            var ServiceInvoice = await _applicationDbContext.ServiceInvoices.FirstOrDefaultAsync(s => s.Id == serviceInvoiceId);

            if (serviceCount == servicesPaid)
            {
                ServiceInvoice.PaymentStatus = "PAID";
                ServiceInvoice.PaymentMethod = services.PaymentMethod;
                ServiceInvoice.DatePaid = DateTime.Now;
            }
            else
            {
                ServiceInvoice.PaymentStatus = "INCOMPLETE";
            }

            await _applicationDbContext.SaveChangesAsync();

            return true;

        }

        public async Task<ServiceRequestResult> UploadServiceRequestResult(ServiceRequestResult serviceRequestResult)
        {
            try
            {
                _applicationDbContext.ServiceRequestResults.Add(serviceRequestResult);
                await _applicationDbContext.SaveChangesAsync();

                return serviceRequestResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UploadServiceRequestResultImage(ServiceUploadResultDto serviceRequestUploadResultDto, string serviceRequestResultId)
        {
           
            try
            {
                if (serviceRequestUploadResultDto != null)
                {
                    for (int i = 0; i < serviceRequestUploadResultDto.Images.Count; i++)
                    {

                        var rootPath = _webHostEnvironment.ContentRootPath;
                        var folderToSaveIn = "wwwroot/Images/";
                        var pathToSave = Path.Combine(rootPath, folderToSaveIn);

                        var absoluteFilePath = "";

                      //  var hostingPath = _hostingEnvironment.WebRootPath,

                        string extension = Path.GetExtension(serviceRequestUploadResultDto.Images[i].FileName);

                        if (ImageValidator.FileSize(_config, serviceRequestUploadResultDto.Images[i].Length) && ImageValidator.Filetype(extension))
                        {
                            if (serviceRequestUploadResultDto.Images != null)
                            {
                                
                                using (var fileStream = new FileStream(Path.Combine(pathToSave, serviceRequestUploadResultDto.Images[i].FileName), FileMode.Create, FileAccess.Write))
                                {
                                   await serviceRequestUploadResultDto.Images[i].CopyToAsync(fileStream);
                                    absoluteFilePath = fileStream.Name;
                                    
                                }

                                // Upload image(s)

                                var image = new ServiceRequestResultImage()
                                {
                                    Image = Path.GetRelativePath(rootPath, absoluteFilePath),
                                    ImageURL = _webHostEnvironment.WebRootFileProvider.GetFileInfo("Images/"+ serviceRequestUploadResultDto.Images[i].FileName)?.PhysicalPath,
                                    ServiceRequestResultId = serviceRequestResultId
                                };
                                _applicationDbContext.ServiceRequestResultImages.Add(image);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                       
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceRequest> GetServiceRequest(string serviceRequestId) => await _applicationDbContext.ServiceRequests.Where(s => s.Id == serviceRequestId).Include(s => s.ServiceInvoice).Include(s => s.Service).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync();

        public async Task<int> GetServiceRequestCount() => await _applicationDbContext.ServiceRequests.Where(s => s.Status == "Awaiting Result").CountAsync();
      
        public async Task<IEnumerable<ServiceRequestResult>> GetServiceRequestResults(string serviceRequestId)
        {

            var serviceRequestResults = await _applicationDbContext.ServiceRequestResults
                .Where(s => s.ServiceRequestId == serviceRequestId).Include(s=>s.ServiceRequestResultImages).Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).OrderBy(s => s.Result).ToListAsync();

            return serviceRequestResults;
        }

        public async Task<bool> DeleteServiceRequest(ServiceRequest serviceRequest)
        {
            try
            {
                if (serviceRequest == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceRequests.Remove(serviceRequest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateServiceRequestInvoice(ServiceInvoice serviceRequestInvoice)
        {
            try
            {
                if (serviceRequestInvoice == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceInvoices.Update(serviceRequestInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateServiceRequest(ServiceRequest ServiceRequest)
        {
            try
            {
                if (ServiceRequest == null)
                {
                    return false;
                }

                _applicationDbContext.ServiceRequests.Update(ServiceRequest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ServiceRequestResult>> GetServiceRequestResultsForPatient(string patientId) => await _applicationDbContext.ServiceRequestResults.Where(s => s.ServiceRequest.ServiceInvoice.PatientId == patientId).Include(s => s.ServiceRequestResultImages).Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).OrderBy(s => s.Result).ToListAsync();

        public PagedList<ServiceRequestResultDtoForView> GetServiceRequestResultsPagination(string serviceRequestId, PaginationParameter paginationParameter)
        {
            var serviceRequestResults = _applicationDbContext.ServiceRequestResults
                .Where(s => s.ServiceRequestId == serviceRequestId).Include(s => s.ServiceRequestResultImages)
                .Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).OrderBy(s => s.Result).ToList();

            var serviceRequestResultsToReturn = _mapper.Map<IEnumerable<ServiceRequestResultDtoForView>>(serviceRequestResults);
            return PagedList<ServiceRequestResultDtoForView>.ToPagedList(serviceRequestResultsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);

        }

        public PagedList<ServiceRequestResultDtoForView> GetServiceRequestResultsForPatientPagination(string patientId, PaginationParameter paginationParameter)
        {
            var serviceRequestResults = _applicationDbContext.ServiceRequestResults.Where(s => s.ServiceRequest.ServiceInvoice.PatientId == patientId).Include(s => s.ServiceRequestResultImages).Include(s => s.ServiceRequest).ThenInclude(s => s.Service).ThenInclude(s => s.ServiceCategory).OrderBy(s => s.Result).ToList();
            var serviceRequestResultsToReturn = _mapper.Map<IEnumerable<ServiceRequestResultDtoForView>>(serviceRequestResults);
            return PagedList<ServiceRequestResultDtoForView>.ToPagedList(serviceRequestResultsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ServiceDtoForView> GetServicesPagnation(PaginationParameter paginationParameter)
        {
            var services = _applicationDbContext.Services.Include(s => s.ServiceCategory).OrderBy(s => s.Name).ToList();

            var servicesToReturn = _mapper.Map<IEnumerable<ServiceDtoForView>>(services);
            return PagedList<ServiceDtoForView>.ToPagedList(servicesToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<ServiceRequestDtoForView> GetServiceRequestsInAnInvoicePagination(string ServiceRequestInvoiceId, PaginationParameter paginationParameter)
        {
            var serviceRequest = _applicationDbContext.ServiceRequests.Where(s => s.ServiceInvoiceId == ServiceRequestInvoiceId)
               .Include(s => s.ServiceInvoice).ThenInclude(i => i.Patient)
               .Include(p => p.Service).ThenInclude(s => s.ServiceCategory).OrderBy(s => s.Service).ToList();

            if (serviceRequest != null)
            {
                var servicesRequestsToReturn = _mapper.Map<IEnumerable<ServiceRequestDtoForView>>(serviceRequest);
                return PagedList<ServiceRequestDtoForView>.ToPagedList(servicesRequestsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
            }
            return null;
        }
    }
}

using HMS.Areas.Nurse.Dtos;
using HMS.Services.Helpers;

namespace HMS.Areas.Interfaces.Nurse
{
    public interface INurse
    {
        PagedList<NurseDtoForView> GetNurses(PaginationParameter paginationParameter);
    }
}

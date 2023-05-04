using System.ComponentModel.DataAnnotations;

namespace LabourCommissioner.Views.Shared.Components.SearchBar
{
    public class SPager
    {

        public SPager()
        {
        }


        public string strServiceId { get; set; }
        public string SearchText { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public int StartRecord { get; set; }
        public int EndRecord { get; set; }

        public long EDistrictId { get; set; }
        public long EVillageId { get; set; }
        public long ETalukaId { get; set; }
        public long EDivisionId { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? EndDate { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public int ApplicationStatus { get; set; }
        public string? MobileNo { get; set; }
        public int UserType { get; set; }
        public long PostId { get; set; }
        public int PageNo { get; set; }

        public string? Search { get; set; }
        public long ServiceId { get; set; }
        public string BeneficiaryId { get; set; }
        public long isApproved { get; set; }
        public int StatusId { get; set; }
        public long SubServiceId { get; set; }

        public SPager(int totalItems, int page, int pageSize = 50)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            //int startPage = currentPage - 5;
            //int endPage = currentPage + 4;
            int startPage = currentPage - 50;
            int endPage = currentPage + 49;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 5)
                {
                    startPage = endPage - (totalPages - 1);
                }
            }
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;

            StartRecord = (CurrentPage - 1) * PageSize + 1;
            EndRecord = StartRecord - 1 + PageSize;


        }

    }
}

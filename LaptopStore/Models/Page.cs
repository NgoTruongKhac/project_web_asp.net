namespace LaptopStore.Models
{
	public class Page
	{
		public int TotalItems { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int StartPage { get; set; }
		public int EndPage { get; set; }

		public Page(int totalItems, int page, int pageSize = 5)
		{
			int totalPages = (int)Math.Ceiling((decimal)(totalItems / (decimal)pageSize));

			int currentPage = page;
			int startPage = currentPage - 5;
			int endPage = currentPage + 4;

			if (startPage <= 0)
			{
				endPage = endPage - (startPage - 1);
				startPage = 1;
			}
			if (endPage > totalPages)
			{
				endPage = totalPages;
				if (endPage > 10)
				{
					startPage = endPage - 9;
				}
			}
			TotalPages = totalPages;
			TotalItems = totalItems;
			PageSize = pageSize;
			StartPage = startPage;
			EndPage = endPage;
			CurrentPage = currentPage;


		}

		public Page()
		{

		}
	}



}

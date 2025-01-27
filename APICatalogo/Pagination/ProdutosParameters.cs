namespace APICatalogo.Pagination;

public class ProdutosParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize;  

    public int PageSize
    {
        get 
        { 
            return _pageSize;
        }

        set 
        {            
            if (_pageSize > maxPageSize)
            {
                _pageSize = maxPageSize;
            }

            _pageSize = value; 
        }
    }
}

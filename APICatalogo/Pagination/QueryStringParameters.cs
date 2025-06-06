﻿namespace APICatalogo.Pagination;

public class QueryStringParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = maxPageSize;

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

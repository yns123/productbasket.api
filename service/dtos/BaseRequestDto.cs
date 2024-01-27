namespace service.dtos;

public class BaseRequestDto
{
    int _pageNumber;
    int _pageLimit;
    string? _orderBy;

    public int PageNumber
    {
        get
        {
            if (_pageNumber <= 0)
                return 1;
            else
                return _pageNumber;
        }
        set
        {
            _pageNumber = value;
        }
    }

    public int PageLimit
    {
        get
        {
            if (_pageLimit <= 0)
                return 20;
            if (_pageLimit > 1000)
                return 1000;
            else
                return _pageLimit;
        }
        set
        {
            _pageLimit = value;
        }
    }
}
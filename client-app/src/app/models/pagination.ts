
export interface Pagination{
    currentPage:number;
    itemsPerPage:number;
    TotalItems:number;
    totalPages:number;
}

export class PaginatedResult<T>{
    data: T;
    Pagination : Pagination;

    constructor(data: T, pagination: Pagination){
        this.data = data;
        this.Pagination = pagination;
    }
}

export class PagingParams {
    pageNumber;
    pageSize;
    constructor(pageNumber = 1, pageSize = 2){
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
    }
}
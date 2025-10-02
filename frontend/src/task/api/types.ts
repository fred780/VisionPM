export interface TaskItem {
    id: string;
    title: string;
    status: number;
    priority: number;
    createdAt: string;
    updatedAt: string;
}

export interface PaginatedTasks {
  items: TaskItem[];
  page: number;
  pageSize: number;
  totalCount: number;
}

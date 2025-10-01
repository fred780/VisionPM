export interface TaskItem {
    id: string;
    title: string;
    status: number;
    priority: number;
    dueDate?: string;
    createdAt: string;
    updatedAt: string;
}
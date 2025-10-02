import React from 'react';
import {
    useCallback,
    useEffect,
    useState,
} from 'react';
import {
    createTask,
    listTasks,
    updateTask,
    deleteTask,
} from './api/api';
import type { TaskItem } from './api/types';
import {
    Button,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    IconButton,
    Pagination,
} from '@mui/material';
import {
    Edit,
    Delete,
} from '@mui/icons-material';
import EditTaskModal from './modal/EditTaskModal';

export default function TaskPage() {
    const [title, setTitle] = useState("");
    const [items, setItems] = useState<TaskItem[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string|undefined>();
    const [editOpen, setEditOpen] = useState(false);
    const [selectedTask, setSelectedTask] = useState<TaskItem | null>(null);

    // Pagination states
    const [page, setPage] = useState(1);
    const [pageSize] = useState(5); // Fixed page size
    const [totalCount, setTotalCount] = useState(0);

    const refresh = useCallback(async (currentPage = page) => {
        const { items, totalCount: apiTotalCount } = await listTasks(currentPage, pageSize);
        setItems(items);
        setTotalCount(apiTotalCount);
        setPage(currentPage);
    }, [page, pageSize])

    function handleEdit(task: TaskItem) {
        setSelectedTask(task);
        setEditOpen(true);
    }

    async function handleDeleteTask(taskId: string) {
        await deleteTask(taskId);
        await refresh();
    }

    async function handleSaveEdit(updated: Partial<TaskItem>) {
        if (!selectedTask) return;
        const updatedTask = {
            ...selectedTask,
            ...updated,
            updatedAt: new Date().toISOString(),
        }
        await updateTask(selectedTask.id, updatedTask);
        await refresh();
        setEditOpen(false);
        setSelectedTask(null);
    }

    useEffect(()=>{
        refresh();
    },[refresh]);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        if (!title.trim()) return;
        try {
            setLoading(true); setError(undefined);
            await createTask({ title, priority: 1 });
            setTitle("");
            await refresh();
        } catch (err: unknown) {
            if (err instanceof Error) {
                setError(err.message);
            } else {
                setError('Error');
            }
        } finally {
            setLoading(false);
        }
    }

    const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
        refresh(value);
    };

    return (
        <div className="container" style={{
            padding: 16,
        }}>
            <h1>To‑Do</h1>
            <form className="row" onSubmit={onSubmit}>
                <input
                    className="input"
                    placeholder="Enter a new task"
                    value={title}
                    onChange={e=>setTitle(e.target.value)}
                />
                <Button className="btn" type="submit" disabled={loading}>Add</Button>
            </form>
            {error && <div className="error">{error}</div>}

            <TableContainer component={Paper} style={{ maxHeight: 500, overflowY: 'auto' }}>
                <Table stickyHeader>
                    <TableHead>
                        <TableRow>
                            <TableCell>Title</TableCell>
                            <TableCell>Priority</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Updated At</TableCell>
                            <TableCell>Created At</TableCell>
                            <TableCell>Action</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {items.map((task) => (
                            <TableRow key={task.id}>
                                <TableCell>{task.title}</TableCell>
                                <TableCell>{task.priority}</TableCell>
                                <TableCell>{task.status}</TableCell>
                                <TableCell>{task.updatedAt ? new Date(task.updatedAt).toLocaleString() : '—'}</TableCell>
                                <TableCell>{new Date(task.createdAt).toLocaleString()}</TableCell>
                                <TableCell>
                                    <div>
                                        <IconButton onClick={() => handleEdit(task)}>
                                            <Edit />
                                        </IconButton>
                                        <IconButton onClick={() => handleDeleteTask(task.id)}>
                                            <Delete />
                                        </IconButton>
                                    </div>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            {/* PAGINATION CONTROL */}
            <div style={{ display: 'flex', justifyContent: 'center', marginTop: 16 }}>
                <Pagination
                    count={Math.ceil(totalCount / pageSize)}
                    page={page}
                    onChange={handlePageChange}
                    color="primary"
                />
            </div>

            {selectedTask && (
                <EditTaskModal
                    open={editOpen}
                    onClose={() => setEditOpen(false)}
                    task={selectedTask}
                    onSave={handleSaveEdit}
                />
            )}
        </div>
    );
}

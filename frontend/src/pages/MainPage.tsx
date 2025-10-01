import { useEffect, useState } from 'react';
import { createTask, listTasks } from '../api';
import type { TaskItem } from '../types';

export default function MainPage(){
    const [title, setTitle] = useState("");
    const [items, setItems] = useState<TaskItem[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string|undefined>();

    async function refresh(){
        const data = await listTasks();
        setItems(data);
    }

    useEffect(()=>{ refresh(); },[]);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        if (!title.trim()) return;
        try {
            setLoading(true); setError(undefined);
            await createTask({ title, priority: 1, dueDate: null, tags: null });
            setTitle("");
            await refresh();
        } catch(err:any) {
            setError(err?.message ?? 'Error');
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="container">
            <h1>To‑Do</h1>
            <form className="row" onSubmit={onSubmit}>
                <input
                    className="input"
                    placeholder="Enter a new task"
                    value={title}
                    onChange={e=>setTitle(e.target.value)}
                />
                <button className="btn" disabled={loading}>Add</button>
            </form>
            {error && <div className="error">{error}</div>}

            <table className="grid">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Status</th>
                        <th>Priority</th>
                        <th>Due Date</th>
                        <th>Created At</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map(t => (
                        <tr key={t.id}>
                            <td>{t.title}</td>
                            <td>{t.status}</td>
                            <td>{t.priority}</td>
                            <td>{t.dueDate?.slice(0,10) ?? '—'}</td>
                            <td>{new Date(t.createdAt).toLocaleString()}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
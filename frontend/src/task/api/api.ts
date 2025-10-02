import type { PaginatedTasks, TaskItem } from "./types";

const BASE = import.meta.env.VITE_API_BASE || "http://localhost:5000";

export async function listTasks(page = 1, pageSize = 10): Promise<PaginatedTasks> {
  try {
    const res = await fetch(`${BASE}/api/tasks?page=${page}&pageSize=${pageSize}`);
    if (!res.ok) throw new Error("Error fetching tasks");
    return await res.json();
  } catch (error) {
    console.error("Error fetching tasks:", error);
    return { items: [], page, pageSize, totalCount: 0 };
  }
}

export async function createTask(payload: Partial<TaskItem>) {
  try {
    const res = await fetch(`${BASE}/api/tasks`, {
      method: "POST",
      headers: { "content-type": "application/json" },
      body: JSON.stringify(payload)
    });
    if (!res.ok) throw new Error("Error creating task");
    return await res.json();
  } catch (error) {
    console.error("Error creating task:", error);
    throw error;
  }
}

export async function updateTask(id: string, payload: Partial<TaskItem>) {
  try {
    const res = await fetch(`${BASE}/api/tasks/${id}`, {
      method: "PUT",
      headers: { "content-type": "application/json" },
      body: JSON.stringify(payload)
    });
    if (!res.ok) throw new Error("Error updating task");
    return await res.json();
  } catch (error) {
    console.error("Error updating task:", error);
    throw error;
  }
}

export async function deleteTask(id: string) {
  try {
    const res = await fetch(`${BASE}/api/tasks/${id}`, {
      method: "DELETE",
    });
    if (!res.ok) throw new Error("Error deleting task");
    return true;
  } catch (error) {
    console.error("Error deleting task:", error);
    throw error;
  }
}

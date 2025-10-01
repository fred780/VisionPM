const BASE = import.meta.env.VITE_API_BASE || "http://localhost:5000";

export async function listTasks() {
  const res = await fetch(`${BASE}/api/tasks`);
  return await res.json();
}


export async function createTask(payload: any) {
  const res = await fetch(`${BASE}/api/tasks`, {
    method: "POST",
    headers: { "content-type": "application/json" },
    body: JSON.stringify(payload)
  });
  if (!res.ok) throw new Error("Error creating task");
  return await res.json();
}

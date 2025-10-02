import React, { useState } from 'react';
import {
  Modal,
  Box,
  Typography,
  Button,
  TextField,
  MenuItem,
} from '@mui/material';
import type { TaskItem } from '../api/types';
import style from './editTaskModalStyle';

interface EditTaskModalProps {
  open: boolean;
  onClose: () => void;
  task: TaskItem;
  onSave: (updated: Partial<TaskItem>) => Promise<void>;
}

export default function EditTaskModal({ open, onClose, task, onSave }: EditTaskModalProps) {
  const [title, setTitle] = useState(task.title || '');
  const [priority, setPriority] = useState(task.priority);
  const [status, setStatus] = useState(task.status);
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    await onSave({
      title,
      priority,
      status,
    });
    setLoading(false);
    onClose();
  }

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <form onSubmit={handleSubmit}>
          <Typography variant="h6" mb={2}>Edit Task</Typography>
          <TextField
            label="Title"
            fullWidth
            value={title}
            onChange={e => setTitle(e.target.value)}
            margin="normal"
          />
          <TextField
            select
            label="Priority"
            fullWidth
            value={priority}
            onChange={e => setPriority(Number(e.target.value))}
            margin="normal"
          >
            <MenuItem value={0}>Low</MenuItem>
            <MenuItem value={1}>Medium</MenuItem>
            <MenuItem value={2}>High</MenuItem>
          </TextField>
          <TextField
            select
            label="Status"
            fullWidth
            value={status}
            onChange={e => setStatus(Number(e.target.value))}
            margin="normal"
          >
            <MenuItem value={0}>Todo</MenuItem>
            <MenuItem value={1}>InProgress</MenuItem>
            <MenuItem value={2}>Done</MenuItem>
          </TextField>
          <Box mt={2} display="flex" justifyContent="flex-end" gap={2}>
            <Button onClick={onClose} disabled={loading}>Cancel</Button>
            <Button variant="contained" type="submit" disabled={loading}>Save</Button>
          </Box>
        </form>
      </Box>
    </Modal>
  );
}

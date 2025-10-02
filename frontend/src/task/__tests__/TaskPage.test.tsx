import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import TaskPage from '../TaskPage';

// Mock API functions
jest.mock('../api/api', () => ({
  listTasks: jest.fn().mockResolvedValue({ items: [], totalCount: 0 }),
  createTask: jest.fn().mockResolvedValue({}),
  updateTask: jest.fn().mockResolvedValue({}),
  deleteTask: jest.fn().mockResolvedValue({}),
}));

describe('TaskPage', () => {
  it('renders the title and add button', async () => {
    render(<TaskPage />);
    expect(await screen.findByText(/To‑Do/i)).toBeInTheDocument();
    expect(await screen.findByRole('button', { name: /Add/i })).toBeInTheDocument();
  });

  it('does not add a task if input is empty', async () => {
    render(<TaskPage />);
    const addButton = await screen.findByRole('button', { name: /Add/i });
    fireEvent.click(addButton);
    expect(screen.queryByText(/error/i)).not.toBeInTheDocument();
  });
});

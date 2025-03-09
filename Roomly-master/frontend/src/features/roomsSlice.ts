import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Room } from "../types/Room";
import { getRooms } from "../api/rooms";

export type RoomsState = {
  rooms: Room[];
  loaded: boolean;
  hasError: string;
  sortBy: keyof Room | null;
  sortOrder: 'asc' | 'desc';
};

const initialState: RoomsState = {
  rooms: [],
  loaded: false,
  hasError: '',
  sortBy: null,
  sortOrder: 'asc',
};

export const initRooms = createAsyncThunk('rooms/fetch', async () => {
  return getRooms();
});

export const roomsSlice = createSlice({
  name: "rooms",
  initialState,
  reducers: {
    sortRooms: (state, action: PayloadAction<keyof Room>) => {
      const column = action.payload;
      if (column === 'description') return;
      
      if (state.sortBy === column) {
        state.sortOrder = state.sortOrder === 'asc' ? 'desc' : 'asc';
      } else {
        state.sortBy = column;
        state.sortOrder = 'asc';
      }
      state.rooms.sort((a, b) => {
        if (a[column] < b[column]) return state.sortOrder === 'asc' ? -1 : 1;
        if (a[column] > b[column]) return state.sortOrder === 'asc' ? 1 : -1;
        return 0;
      });
    },
  },
  extraReducers: builder => {
    builder.addCase(initRooms.pending, state => {
      state.loaded = false;
      state.hasError = '';
    });
    builder.addCase(initRooms.fulfilled, (state, action: PayloadAction<Room[]>) => {
      state.rooms = action.payload;
      state.loaded = true;
    });
    builder.addCase(initRooms.rejected, (state, action) => {
      state.hasError = action.error.message || 'Failed to load rooms';
      state.loaded = true;
    });
  },
});

export const { sortRooms } = roomsSlice.actions;
export default roomsSlice.reducer;

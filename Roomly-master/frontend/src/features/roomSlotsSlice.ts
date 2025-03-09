import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import { getSelectedRoom } from '../api/rooms';
import { RoomSlot } from '../types/RoomSlot';

export type RoomSlotsState = {
  slots: RoomSlot[];
  loaded: boolean;
  hasError: string;
};

const initialState: RoomSlotsState = {
  slots: [],
  loaded: false,
  hasError: '',
};

export const fetchRoomSlots = createAsyncThunk('roomSlots/fetch', async (roomId: string) => {
  return getSelectedRoom(roomId);
});

export const roomSlotsSlice = createSlice({
  name: 'roomSlots',
  initialState,
  reducers: {
    clearSlots: (state) => {
      state.slots = [];
      state.loaded = false;
      state.hasError = '';
    }
  },
  extraReducers: (builder) => {
    builder.addCase(fetchRoomSlots.pending, (state) => {
      state.loaded = false;
      state.hasError = '';
    });
    builder.addCase(fetchRoomSlots.fulfilled, (state, action: PayloadAction<RoomSlot[]>) => {
      state.slots = action.payload;
      state.loaded = true;
    });
    builder.addCase(fetchRoomSlots.rejected, (state, action) => {
      state.hasError = action.error.message || 'Failed to load slots';
      state.loaded = true;
    });
  }
});

export const { clearSlots } = roomSlotsSlice.actions;
export default roomSlotsSlice.reducer;

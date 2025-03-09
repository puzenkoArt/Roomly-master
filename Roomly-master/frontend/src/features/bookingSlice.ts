import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { BookingRoom } from "../types/BookingRoom";
import { getBookings, createBooking, updateBookingStatus } from "../api/booking";

export type BookingRoomState = {
  bookings: BookingRoom[];
  loaded: boolean;
  hasError: string;
};

const initialState: BookingRoomState = {
  bookings: [],
  loaded: false,
  hasError: '',
};

export const initBookings = createAsyncThunk('bookings/fetch', async () => {
  return getBookings();
});

export const addBooking = createAsyncThunk(
  'bookings/add',
  async (newBooking: BookingRoom) => {
    return createBooking(newBooking);
  }
);

export const cancelBooking = createAsyncThunk(
  "bookings/remove",
  async (bookingId: string) => {
    await updateBookingStatus(bookingId);
    return bookingId;
  }
);

export const bookingSlice = createSlice({
  name: "bookings",
  initialState,
  reducers: {
    clearBookings: (state) => {
      state.bookings = [];
      state.loaded = false;
      state.hasError = "";
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(initBookings.pending, (state) => {
        state.loaded = false;
        state.hasError = "";
      })
      .addCase(initBookings.fulfilled, (state, action: PayloadAction<BookingRoom[]>) => {
        state.bookings = action.payload;
        state.loaded = true;
      })
      .addCase(initBookings.rejected, (state, action) => {
        state.hasError = action.error.message || "Failed to load bookings";
        state.loaded = true;
      })

      .addCase(addBooking.fulfilled, (state, action: PayloadAction<BookingRoom>) => {
        state.bookings.push(action.payload);
      })
      .addCase(addBooking.rejected, (state, action) => {
        state.hasError = action.error.message || "Failed to create booking";
      })
      .addCase(cancelBooking.fulfilled, (state, action: PayloadAction<string>) => {
        const booking = state.bookings.find(b => b.bookingId === action.payload);
        if (booking) {
          booking.status = "Cancelled";
        }
      })      
      .addCase(cancelBooking .rejected, (state, action) => {
        state.hasError = action.error.message || "Failed to delete booking";
      });
  },
});

export default bookingSlice.reducer;

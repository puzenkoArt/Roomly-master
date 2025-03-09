/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { login, register, logout } from "../api/auth";
import { Login, Register, User } from "../types/Auth";

interface AuthState {
  token: string | null;
  user: User | null;
  loading: boolean;
  error: string | null;
}

const storedToken = localStorage.getItem("token") || null;
const storedUser = localStorage.getItem("user");

const initialState: AuthState = {
  token: storedToken,
  user: storedUser ? JSON.parse(storedUser) : null,
  loading: false,
  error: null,
};

export const loginUser = createAsyncThunk(
  "auth/loginUser",
  async (credentials: Login, { rejectWithValue }) => {
    try {
      const response: any = await login(credentials);

      if (typeof response === "string" && response.includes(".") && response.split(".").length === 3) {
        return { token: response };
      } else if (typeof response === "object" && response.token && typeof response.token === "string" &&
                 response.token.includes(".") && response.token.split(".").length === 3) {
        return response;
      } else {
        return rejectWithValue(response || "Authentication error");
      }
    } catch (error: any) {
      return rejectWithValue("Error network or server");
    }
  }
);


export const registerUser = createAsyncThunk(
  "auth/registerUser",
  async (userData: Register, { rejectWithValue }) => {
    try {
      const response = await register(userData);
      return response;
    } catch (error: any) {
      console.error("Error in registerUser:", error.message);

      try {
        const parsedError = JSON.parse(error.message);
        if (Array.isArray(parsedError)) {
          const errorMessages = parsedError.map(err => err.description).join("\n");
          return rejectWithValue(errorMessages);
        }
      } catch {
        return rejectWithValue("Register error");
      }

      return rejectWithValue("Register error");
    }
  }
);

export const logoutUser = createAsyncThunk("auth/logoutUser", async (_, { rejectWithValue }) => {
  try {
    await logout();
    return null;
  } catch (error: any) {
    return rejectWithValue(error.message);
  }
});

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.loading = false;
        state.token = action.payload.token;
    
        if (action.payload.user) {
          state.user = action.payload.user;
          localStorage.setItem("user", JSON.stringify(action.payload.user));
        }
        localStorage.setItem("token", action.payload.token);
      }) 
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(registerUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(registerUser.fulfilled, (state, action) => {
        state.loading = false;
        
        if (action.payload.token) {
          state.token = action.payload.token;
          if (action.payload.user) {
            state.user = action.payload.user;
          }
        }
      })
      .addCase(registerUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      })
      .addCase(logoutUser.fulfilled, (state) => {
        state.loading = false;
        state.token = null;
        state.user = null;
        
        localStorage.removeItem("token");
        localStorage.removeItem("user");
    })          
      .addCase(logoutUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});


export const { clearError } = authSlice.actions;
export default authSlice.reducer;

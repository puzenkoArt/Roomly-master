import React, { useState, useEffect, useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { clearError, loginUser } from "../../features/authSlice";
import { Link, useNavigate, useLocation } from "react-router-dom";

export const LoginPage = () => {
  const navigate = useNavigate();
  const { state } = useLocation();
  const dispatch = useAppDispatch();
  const { loading, error, token } = useAppSelector((state) => state.auth);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  useEffect(() => {
    dispatch(clearError());
  }, [dispatch]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(loginUser({ email, password }));
  };

  const redirectPath = useMemo(() => state?.pathname || "/rooms", [state?.pathname]);

  useEffect(() => {
    if (token) {
      navigate(redirectPath, { replace: true });
    }
  }, [token, navigate, redirectPath]);

  return (
    <div className="container is-flex is-justify-content-center is-align-items-center" style={{ height: "100vh" }}>
      <div className="box" style={{ width: "400px" }}>
        <h1 className="title has-text-centered">Login</h1>
        <form onSubmit={handleSubmit}>
          <div className="field">
            <label className="label">Email</label>
            <div className="control has-icons-left">
              <input
                className="input"
                type="email"
                placeholder="Enter your email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <span className="icon is-small is-left">
                <i className="fas fa-envelope"></i>
              </span>
            </div>
          </div>

          <div className="field">
            <label className="label">Password</label>
            <div className="control has-icons-left">
              <input
                className="input"
                type="password"
                placeholder="Enter your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <span className="icon is-small is-left">
                <i className="fas fa-lock"></i>
              </span>
            </div>
          </div>

          {error && <p className="has-text-danger has-text-centered">{error}</p>}

          <div className="field mt-4">
            <button className="button is-primary is-fullwidth" type="submit" disabled={loading}>
              {loading ? "Logging in..." : "Login"}
            </button>
          </div>
        </form>

        <p className="has-text-centered mt-3">
          Don't have an account? <Link to="/register" className="has-text-link">Register</Link>
        </p>
      </div>
    </div>
  );
};

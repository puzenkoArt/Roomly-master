import { NavLink, Outlet } from "react-router-dom";
import classNames from "classnames";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import { logoutUser } from "./features/authSlice";
import { useEffect } from "react";

const getLinkActiveClass = ({ isActive }: { isActive: boolean }) =>
  classNames("navbar-item", {
    "has-background-grey-lighter": isActive,
  });

export const App = () => {
  const dispatch = useAppDispatch();
  const { token } = useAppSelector((state) => state.auth);

  const handleLogout = () => {
    dispatch(logoutUser());
  };

  useEffect(() => {
    const storedToken = localStorage.getItem("token");

    if (storedToken && !token) {
      dispatch({ type: "auth/setToken", payload: storedToken });
    }
  }, [token, dispatch]);

  return (
    <div data-cy="app">
      <nav
        data-cy="nav"
        className="navbar is-fixed-top has-shadow"
        role="navigation"
        aria-label="main navigation"
      >
        <div className="container">
          <div className="navbar-menu is-active">
            <div className="navbar-start">
              <NavLink className={getLinkActiveClass} to="/">Home</NavLink>
              <NavLink className={getLinkActiveClass} to="/rooms">Rooms</NavLink>
            </div>
  
            <div className="navbar-end">
              {token ? (
                <>
                  <NavLink className={getLinkActiveClass} to="/account">Account</NavLink>
                  <button className="button is-danger" onClick={handleLogout}>
                    Logout
                  </button>
                </>
              ) : (
                <NavLink className={getLinkActiveClass} to="/login">Login</NavLink>
              )}
            </div>
          </div>
        </div>
      </nav>
  
      <Outlet />
    </div>
  );  
};

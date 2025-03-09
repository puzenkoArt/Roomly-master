import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAppSelector } from "../../app/hooks";

export const RequireAuth = () => {
  const { token } = useAppSelector((state) => state.auth);
  const { pathname } = useLocation();

  if (!token) {
    return <Navigate to="/login" state={{ pathname }} replace/>;
  }

  return <Outlet />;
};

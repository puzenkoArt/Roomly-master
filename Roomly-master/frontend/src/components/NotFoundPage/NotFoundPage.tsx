import { Link } from "react-router-dom";

export const NotFoundPage = () => (
  <section className="section has-text-centered">
    <div className="container">
      <h1 className="title is-1 has-text-danger">404</h1>
      <p className="subtitle is-4">Oops! Page not found.</p>
      <p className="mb-5">
        The page you are looking for doesn't exist or has been moved.
      </p>
      <Link to="/home" className="button is-info is-large">
        <span className="icon">
          <i className="fas fa-home"></i>
        </span>
        <span>Go Home</span>
      </Link>
    </div>
  </section>
);

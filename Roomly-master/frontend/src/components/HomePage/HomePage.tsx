import { Link } from "react-router-dom";
import meetingRoom from "../../img/meeting-room.jpg";

export const HomePage = () => (
  <div className="section">
    <br />
    <br />
    <div className="container has-text-centered">
      <h1 className="title is-2">Welcome to the Meeting Room Booking System</h1>
      <br />
      <p className="subtitle is-5">
        Easily book and manage meeting rooms for your team.
      </p>

      <div className="columns is-centered mt-5">
        <div className="column is-half">
          <figure className="image is-16by9">
            <img src={meetingRoom} alt="Meeting Room" />
          </figure>
        </div>
      </div>

      <div className="buttons is-centered mt-5">
        <Link to="/rooms" className="button is-info is-large">
          Book a Room
        </Link>
        <Link to="/account" className="button is-warning is-large">
          My Bookings
        </Link>
      </div>

      <div className="box mt-6">
        <h2 className="title is-4">Why Choose Us?</h2>
        <div className="columns">
          <div className="column">
            <div className="notification is-info">
              <strong>Easy to Use</strong> – Book a room in just a few clicks.
            </div>
          </div>
          <div className="column">
            <div className="notification is-success">
              <strong>Flexible</strong> – Modify or cancel bookings anytime.
            </div>
          </div>
          <div className="column">
            <div className="notification is-warning">
              <strong>Real-Time Updates</strong> – Always fresh information.
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
);

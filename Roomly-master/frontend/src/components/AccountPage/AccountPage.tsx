import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { Loader } from "../Loader";
import { initBookings, cancelBooking } from "../../features/bookingSlice";
import { initRooms } from "../../features/roomsSlice";
import { Room } from "../../types/Room";
import { getRoomType } from "../RoomsPage";
import cn from "classnames";

export const AccountPage = () => {
  const dispatch = useAppDispatch();
  const { bookings, loaded, hasError } = useAppSelector(
    (state) => state.booking
  );
  const { rooms } = useAppSelector((state) => state.rooms);

  useEffect(() => {
    dispatch(initBookings());
    dispatch(initRooms());
  }, [dispatch]);

  const bookedRooms = bookings
    .map((booking) => rooms.find((room) => room.id === booking.roomId))
    .filter((room): room is Room => !!room);

  const handleCancelBooking = (bookingId: string) => {
    dispatch(cancelBooking(bookingId));
  };

  const getStatusText = (status: number | string): string => {
    if (typeof status === "string") return status;
    switch (status) {
      case 0:
        return "Confirmed";
      case 1:
        return "OnApproval";
      case 2:
        return "Cancelled";
      default:
        return "Unknown";
    }
  };

  return (
    <main className="section">
      <div className="container">
        <br />
        <br />
        <h1 className="title">Booking Rooms</h1>
        <div className="box">
          {!loaded && <Loader />}
          {hasError && (
            <p className="has-text-danger">Something went wrong: {hasError}</p>
          )}
          {loaded && bookedRooms.length === 0 && !hasError && (
            <p>No booking meeting rooms</p>
          )}
          {loaded && bookedRooms.length > 0 && (
            <div className="table-container">
              <table className="table is-fullwidth is-striped is-hoverable">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Capacity</th>
                    <th>Location</th>
                    <th>Type</th>
                    <th>Time</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {bookings.map((booking) => {
                    const room = rooms.find((r) => r.id === booking.roomId);
                    if (!room) return null;
                    const formattedTime = `${new Date(
                      booking.startTime
                    ).toLocaleTimeString()} - ${new Date(
                      booking.endTime
                    ).toLocaleTimeString()}`;
                    const statusText = getStatusText(booking.status!);
                    return (
                      <tr key={`${booking.roomId}-${booking.startTime}`}>
                        <td>{room.name}</td>
                        <td>{room.capacity}</td>
                        <td>{room.location}</td>
                        <td>{getRoomType(room.type)}</td>
                        <td>{formattedTime}</td>
                        <td>
                          <span
                            className={`tag ${
                              statusText === "Cancelled"
                                ? "is-danger"
                                : "is-success"
                            }`}
                          >
                            {statusText}
                          </span>
                        </td>
                        <td>
                          <button
                            className={cn("button is-small", {
                              "is-light": statusText === "Cancelled",
                              "is-danger": statusText !== "Cancelled",
                            })}
                            onClick={() =>
                              handleCancelBooking(booking.bookingId!)
                            }
                            disabled={statusText === "Cancelled"}
                          >
                            {statusText === "Cancelled"
                              ? "Canceled"
                              : "Cancel Booking"}
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </main>
  );
};

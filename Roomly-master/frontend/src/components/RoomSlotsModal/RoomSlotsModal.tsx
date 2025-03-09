import React, { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { clearSlots, fetchRoomSlots } from "../../features/roomSlotsSlice";
import { RoomSlot } from "../../types/RoomSlot";
import { BookingRoom } from "../../types/BookingRoom";
import { Loader } from "../Loader";
import { addBooking } from "../../features/bookingSlice";
import cn from "classnames";

type Props = {
  roomId: string | null;
  roomName: string | null;
  onClose: () => void;
};

export const RoomSlotsModal: React.FC<Props> = ({
  roomId,
  roomName,
  onClose,
}) => {
  const dispatch = useAppDispatch();
  const { slots, loaded, hasError } = useAppSelector((state) => state.roomSlots);
  const [bookedSlots, setBookedSlots] = useState<Set<string>>(new Set());

  const onClick = (newBooking: BookingRoom) => {
    dispatch(addBooking(newBooking));
    setBookedSlots((prev) => new Set(prev).add(newBooking.startTime));
  };

  useEffect(() => {
    if (roomId) {
      dispatch(fetchRoomSlots(roomId));
    }
    return () => {
      dispatch(clearSlots());
      setBookedSlots(new Set());
    };
  }, [roomId, dispatch]);

  if (!roomId) return null;

  return (
    <div className="modal is-active">
      <div className="modal-background" onClick={onClose}></div>
      <div className="modal-content">
        <div className="box">
          <h2 className="is-size-3 has-text-weight-bold has-text-centered">
            Available Slots for {roomName}
          </h2>
          {!loaded && <Loader />}

          {hasError && (
            <p
              data-cy="roomsLoadingError"
              className="has-text-danger has-text-centered"
            >
              Something went wrong: {hasError}
            </p>
          )}

          {loaded && slots.length === 0 && !hasError && (
            <p data-cy="noRoomsMessage" className="has-text-centered">
              No available slots
            </p>
          )}

          {loaded && slots.length > 0 && (
            <div className="is-flex is-flex-direction-column">
              {slots.map((slot: RoomSlot) => {
                const isBooked = bookedSlots.has(slot.startTime);

                return (
                  <div
                    key={slot.startTime}
                    className="is-flex is-align-items-center is-justify-content-space-between p-2"
                    style={{ borderBottom: "1px solid #ddd" }}
                  >
                    <span className="is-size-5">
                      {new Date(slot.startTime).toLocaleTimeString()} - {" "}
                      {new Date(slot.endTime).toLocaleTimeString()}
                    </span>
                    <div className="is-flex is-align-items-center">
                      <span
                        className={cn("tag is-medium has-text-centered", {
                          "is-danger": isBooked,
                          "is-success is-light": !isBooked,
                        })}
                        style={{ marginRight: "0.5rem" }}
                      >
                        {isBooked ? "Disabled" : "Available"}
                      </span>
                      <button
                        className="button is-small is-primary has-text-weight-bold"
                        onClick={() =>
                          onClick({
                            roomId: roomId!,
                            startTime: slot.startTime,
                            endTime: slot.endTime,
                          })
                        }
                        disabled={isBooked}
                      >
                        {isBooked ? "Booked" : "Book"}
                      </button>
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>
      <button className="modal-close is-large" aria-label="close" onClick={onClose}></button>
    </div>
  );
};
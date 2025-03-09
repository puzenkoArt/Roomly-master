import { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { initRooms, sortRooms } from '../../features/roomsSlice';
import { Loader } from '../Loader';
import { Room } from '../../types/Room';
import { RoomSlotsModal } from '../RoomSlotsModal/RoomSlotsModal';

export const getRoomType = (type: number) => (type === 0 ? 'Meeting Room' : 'Workplace');

export const RoomsPage = () => {
  const dispatch = useAppDispatch();
  const { rooms, loaded, hasError, sortBy, sortOrder } = useAppSelector(state => state.rooms);
  const [selectedRoomId, setSelectedRoomId] = useState<string | null>(null);
  const [selectedRoomName, setSelectedRoomName] = useState<string | null>(null);

  useEffect(() => {
    dispatch(initRooms());
  }, [dispatch]);

  const handleSort = (column: keyof Room) => {
    if (column !== 'description') {
      dispatch(sortRooms(column));
    }
  };

  const getColumnTitle = (key: string) => {
    return `${key.charAt(0).toUpperCase() + key.slice(1)} ${sortBy === key ? (sortOrder === 'asc' ? '' : '') : ''}`;
  };

  const handleViewSlots = (roomId: string, roomName: string) => {
    setSelectedRoomId(roomId);
    setSelectedRoomName(roomName);
  };

  return (
    <main className="section">
      <div className="container">
        <br />
        <br />
        <h1 className="title">Meeting Rooms</h1>
        <div className="block">
          <div className="box table-container">
            {!loaded && <Loader />}

            {hasError && (
              <p data-cy="roomsLoadingError" className="has-text-danger">
                Something went wrong: {hasError}
              </p>
            )}

            {loaded && rooms.length === 0 && !hasError && (
              <p data-cy="noRoomsMessage">No available meeting rooms</p>
            )}

            {loaded && rooms.length > 0 && (
              <table className="table is-fullwidth is-striped">
                <thead>
                  <tr>
                    {['name', 'capacity', 'location', 'type'].map((key) => (
                      <th key={key} className="is-clickable" onClick={() => handleSort(key as keyof Room)}>
                        {getColumnTitle(key)}
                      </th>
                    ))}
                    <th>Description</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {rooms.map((room) => (
                    <tr key={room.id}>
                      <td>{room.name}</td>
                      <td>{room.capacity}</td>
                      <td>{room.location}</td>
                      <td>{getRoomType(room.type)}</td>
                      <td>{room.description}</td>
                      <td>
                        <button
                          className="button"
                          onClick={() => handleViewSlots(room.id, room.name)}
                        >
                          View Slots
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        </div>
      </div>

      <RoomSlotsModal
        roomId={selectedRoomId}
        roomName={selectedRoomName}
        onClose={() => {
          setSelectedRoomId(null);
          setSelectedRoomName(null);
        }}
      />
    </main>
  );
};

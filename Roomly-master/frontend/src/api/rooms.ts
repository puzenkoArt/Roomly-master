import { Room } from '../types/Room';
import { RoomSlot } from '../types/RoomSlot';
import { client } from '../utils/fetchClient';

export const createRoom = (data: Omit<Room, 'id'>) => {
  return client.post<Room>('/rooms', data);
};

export const getSelectedRoom = (roomId: string) => {
  return client.get<RoomSlot[]>(`/rooms/${roomId}/slots`);
};

export const getRooms = () => {
  return client.get<Room[]>('/rooms');
};

import { RoomType } from "./RoomType";

export type Room = {
  id: string;
  name: string;
  location: string;
  capacity: number;
  type: RoomType;
  description: string;
}

import { Photo } from "./Photo";

  export interface member {
      id: number;
      userName: string;
      photoUrl: string;
      age: number;
      knownAs: string;
      create: Date;
      lastActive: Date;
      gender: string;
      interduction?: any;
      lookingFor: string;
      interests: string;
      city: string;
      country: string;
      photos: Photo[];
  }




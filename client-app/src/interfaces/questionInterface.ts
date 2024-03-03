import Concurso from './concursoInterface';
//import Level from './levelInterface';
import Subject from './subjectInterface';
import { User } from './userInterface';

export interface Question {
  id: number;
  concurso: Concurso;
  level: string;
  instituteName: string;
  subject: Subject;
  body: string;
  answer?: 'A' | 'B' | 'C' | 'D' | 'E';
  choices?: { id: number; letter: string; text: string }[];
  tip: string;
  insertedBy: User;
  insertedAt: string;
  lastUpdateBy?: User;
  lastUpdateAt?: string;
}

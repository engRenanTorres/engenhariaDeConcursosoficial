import Concurso from './concursoInterface';
import Level from './levelInterface';
import Subject from './subjectInterface';
import { User } from './userInterface';

export interface Question {
  id: number;
  concurso: Concurso;
  level: Level;
  subject: Subject;
  question: string;
  answer?: 'A' | 'B' | 'C' | 'D' | 'E';
  choices?: { id: number; choice: string }[];
  tip: string;
  InsertedBy: User;
  InsertedAt: string;
  lastUpdateBy?: User;
  lastUpdateAt?: string;
}

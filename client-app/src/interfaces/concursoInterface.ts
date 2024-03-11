//import Institute from './instituteInterface';

import Institute from './instituteInterface';

export default interface Concurso {
  id: number;
  name: string;
  about: string;
  year: number;
  //institute: Institute;
}
export interface CreateConcurso {
  id: number;
  name: string;
  about: string;
  year: number;
  institute: Institute;
}

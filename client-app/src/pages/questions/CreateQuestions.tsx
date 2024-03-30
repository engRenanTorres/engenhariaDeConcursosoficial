import { useNavigate } from 'react-router-dom';
import QuestionsList from '../../containers/questions/CreateQuestions';
import axiosClient from '../../utils/httpClient/axiosClient';
import QuestionLayout from '../../components/layout/QuestionLayout';

export interface FormikCreateQuestionValues {
  question: string;
  choicesQtd: number;
  alternative1: string;
  alternative2: string;
  alternative3: string;
  alternative4: string;
  alternative5: string;
  concursoId: string;
  // area: string;
  subjectId: string;
  levelId: string;
  answer: string;
  tip: string;
}

function CreateQuestions() {
  const navigate = useNavigate();
  const handleSubmit = async (values: FormikCreateQuestionValues) => {
    // console.log(values);
    const choices = [];
    choices.push({
      choice: values.alternative1 === '' ? 'Correta' : values.alternative1,
    });
    choices.push({
      choice: values.alternative2 === '' ? 'Errada' : values.alternative2,
    });
    if (values.alternative3 !== '')
      choices.push({ choice: values.alternative3 });
    if (values.alternative4 !== '')
      choices.push({ choice: values.alternative4 });
    if (values.alternative5 !== '')
      choices.push({ choice: values.alternative5 });

    const { status } = await axiosClient.post('/question', {
      body: values.question,
      answer: values.answer,
      tip: values.tip,
      choices: choices,
      levelId: values.levelId,
      subjectId: values.subjectId,
      concursoId: values.concursoId,
    });

    if (status === 201) {
      alert('Questão criada com sucesso.');
      navigate('/');
    } else {
      alert(`erro ao criar a questão status: ${status}`);
    }
  };
  return (
    <QuestionLayout
      jumbotronTitle="Engenharia de concursos"
      jumbotronSubtitle="Simulador de concursos de engenharia"
    >
      <QuestionsList handleSubmit={handleSubmit} />
    </QuestionLayout>
  );
}

export default CreateQuestions;

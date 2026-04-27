import { JOB_STATES, CANCELLED_STEP } from '@/utils/jobState';

import OfferStepContent from '@/components/dashboard/jobs/steps/ExpertOfferStepContent';
import CancelOnlyStepContent from '@/components/dashboard/jobs/steps/CancelOnlyStepContent';
import JobProgressionStepContent from '@/components/dashboard/jobs/steps/ExpertJobProgressionStepContent';
import UserOfferedStepContent from '@/components/dashboard/jobs/steps/UserOfferedStepContent';
import UserJobProgressionStepContent from '@/components/dashboard/jobs/steps/UserJobProgressionStepContent';
import UserCompletedStepContent from '@/components/dashboard/jobs/steps/UserCompletedStepContent';

export { CANCELLED_STEP };

export const EXPERT_STEPS = [
  {
    state: JOB_STATES.Pending,
    label: 'Ajánlat készítése',
    component: OfferStepContent,
    expandWhenPast: true,
  },
  {
    state: JOB_STATES.Offered,
    label: 'Várakozás az ajánlat elfogadására',
    component: CancelOnlyStepContent,
    expandWhenPast: false,
    extraProps: {
      info: 'Várjuk a megrendelő döntését.',
      buttonLabel: 'Ajánlat visszavonása',
      confirmMessage: 'Biztosan vissza szeretnéd vonni az ajánlatot?',
    },
  },
  {
    state: JOB_STATES.Accepted,
    label: 'Munka folyamatban',
    component: JobProgressionStepContent,
    expandWhenPast: true,
  },
  {
    state: JOB_STATES.Completed,
    label: 'Munka befejezve',
    component: null,
    expandWhenPast: false,
  },
];

export const USER_STEPS = [
  {
    state: JOB_STATES.Pending,
    label: 'Ajánlatra várakozás',
    component: CancelOnlyStepContent,
    expandWhenPast: false,
    extraProps: {
      buttonLabel: 'Munkakérés lemondása',
      confirmMessage: 'Biztosan le szeretnéd mondani a munkakérést?',
    },
  },
  {
    state: JOB_STATES.Offered,
    label: 'Ajánlat elfogadása',
    component: UserOfferedStepContent,
    expandWhenPast: true,
  },
  {
    state: JOB_STATES.Accepted,
    label: 'Munka folyamatban',
    component: UserJobProgressionStepContent,
    expandWhenPast: true,
  },
  {
    state: JOB_STATES.Completed,
    label: 'Munka befejezve',
    component: UserCompletedStepContent,
    expandWhenPast: false,
  },
];

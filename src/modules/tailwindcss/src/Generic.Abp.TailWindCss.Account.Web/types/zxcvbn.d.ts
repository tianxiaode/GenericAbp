   declare module 'zxcvbn-lite' {
     export default function zxcvbn(password: string, userInputs?: string[]): {
       score: number;
       feedback: {
         warning: string | null;
         suggestions: string[];
         passwordStrengthWord: string;
       };
       guesses_log10: number
       // 其他zxcvbn返回的对象属性...
     };
   }
   

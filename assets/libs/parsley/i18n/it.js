// Validation errors messages for Parsley
// Load this after Parsley

Parsley.addMessages('it', {
  defaultMessage: "Questo valore sembra essere non valido.",
  type: {
    email:        "Email non valida",
    url:          "URL non valido",
    number:       "Questo valore deve essere un numero valido.",
    integer:      "Questo valore deve essere un numero valido.",
    digits:       "Questo valore deve essere di tipo numerico.",
    alphanum:     "Questo valore deve essere di tipo alfanumerico."
  },
  notblank:       "Questo valore non deve essere vuoto.",
  required:       "Valore richiesto",
  pattern:        "Valore non corretto",
  min:            "Almeno %s caratteri",
  max:            "Massimo %s caratteri",
  range:          "Questo valore deve essere compreso tra %s e %s.",
  minlength:      "Questo valore è troppo corto. La lunghezza minima è di %s caratteri.",
  maxlength:      "Questo valore è troppo lungo. La lunghezza massima è di %s caratteri.",
  length:         "La lunghezza di questo valore deve essere compresa fra %s e %s caratteri.",
  mincheck:       "Devi scegliere almeno %s opzioni.",
  maxcheck:       "Devi scegliere al più %s opzioni.",
  check:          "Devi scegliere tra %s e %s opzioni.",
  equalto:        "Questo valore deve essere identico.",
  euvatin:        "Non è un codice IVA valido",
});

Parsley.setLocale('it');

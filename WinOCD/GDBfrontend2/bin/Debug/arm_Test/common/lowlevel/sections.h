#ifndef SECTIONSH
#define SECTIONSH

 //vor die Funktion setzen z.B. FASTRUN void bla(...){} dann
 //wird diese Funktion aus dem RAM ausgeführt (->30% schneller)
 #define SEC_FASTRUN __attribute__ ((long_call, section (".fastrun")))

#endif

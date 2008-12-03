#ifndef serial_h_
#define serial_h_

void init_dbgu ( void ); // inits with 115200 Baud
void dbgu_putc(char ch); 
int  dbgu_putchar (int ch);
int  dbgu_puts(char *s);
int  dbgu_kbhit( void );
int  dbgu_getc ( void );



//for rprintf:
#define DBG dbgu_putc
#endif


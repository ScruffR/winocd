/******************************************************************************/
/*  This file has been a part of the uVision/ARM development tools            */
/*  Copyright KEIL ELEKTRONIK GmbH 2002-2004                                  */
/******************************************************************************/
/*                                                                            */
/*  SERIAL.C:  Low Level Serial Routines                                      */
/*                                                                            */
/******************************************************************************/

/* 
   Martin Thomas : 
   - slightly modified for the WinARM example 
   - changed function names to avoid conflict with "stdio"
     (so far no syscalls for the newlib in this example)
   - Keil seems to "reject" the AT91lib*.h. Don't know
     why since the layer is "thin". Maybe will change this to 
	 "lib Style" in later version...
*/

#include "board.h"

#define BR    115200                        /* Baud Rate */
#define BRD  (MCK/16/BR)                    /* Baud Rate Divisor */

// Low Level Funktionen (aber putchar, puts und printf in Tasks benutzen! (reentrant))
void init_dbgu(void) {                   /* Initialize Serial Interface */

   AT91C_BASE_PMC->PMC_PCER = (( 1 << AT91C_ID_SYS ));
   
  AT91C_BASE_PIOA->PIO_PDR = AT91C_PIO_PA9 |        /* Enable DBGU RX Pin */
                    AT91C_PIO_PA10;         /* Enalbe DBGU TX Pin */

  AT91C_BASE_DBGU->DBGU_CR = AT91C_US_RSTRX |          /* Reset Receiver      */
                             AT91C_US_RSTTX |          /* Reset Transmitter   */
                             AT91C_US_RXDIS |          /* Receiver Disable    */
                             AT91C_US_TXDIS;           /* Transmitter Disable */

  AT91C_BASE_DBGU->DBGU_MR = AT91C_US_USMODE_NORMAL |  /* Normal Mode */
                    AT91C_US_PAR_NONE;  /* No Parity   */
   

  AT91C_BASE_DBGU->DBGU_BRGR = BRD;                    /* Baud Rate Divisor */

     
  AT91C_BASE_DBGU->DBGU_CR = AT91C_US_RXEN  |          /* Receiver Enable     */
                           AT91C_US_TXEN;            /* Transmitter Enable  */
                  
  // RX Interrupt anschalten -> Wirft den "system interrupt", der auch vom FreeRTOS (PID-Timer) benutzt wird
  // Daher muss die Funktion vApplicationHook funktion benutzt werden, um in diesem Fall im Interrupt keinen Kontextwechsel
  // sondern eine Verarbeitung des DBGU-RXRDY Interrupts zu ermöglichen!  
   AT91C_BASE_DBGU->DBGU_IER = AT91C_US_RXRDY;           /* AT91C_US_RXRDY Interrupt enable*/ 
   
  //setvbuf(stdout,NULL,_IONBF,0); //use no buffer used for printf on stdout (NewLib), T.R.
}

void dbgu_putc(char ch) 
{
	while (!(AT91C_BASE_DBGU->DBGU_CSR & AT91C_US_TXRDY));   /* Wait for Empty Tx Buffer */
    AT91C_BASE_DBGU->DBGU_THR = ch;                 /* Transmit Character */
}	

int dbgu_putchar (int ch) {                      /* Write Character to Serial Port */

  //if (ch == '\n')  {                            /* Check for LF */
  //  dbgu_putc( '\r' );                         /* Output CR */
  //}
  dbgu_putc( ch );
  return ch;                     /* Transmit Character */
}

int dbgu_puts ( char* s )
{
	while ( *s ) dbgu_putchar( *s++ );
	return 0;
}

int dbgu_kbhit( void ) /* returns true if character in receive buffer */
{
	if ( AT91C_BASE_DBGU->DBGU_CSR & AT91C_US_RXRDY) {
		return 1;
	}
	else {
		return 0;
	}
}

int dbgu_getc ( void )  /* Read Character from Serial Port */
{    

  while (!(AT91C_BASE_DBGU->DBGU_CSR & AT91C_US_RXRDY));   /* Wait for Full Rx Buffer */
  return (AT91C_BASE_DBGU->DBGU_RHR);                      /* Read Character */
}


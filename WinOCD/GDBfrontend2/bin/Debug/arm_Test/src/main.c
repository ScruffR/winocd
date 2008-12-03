#include "board.h"
#include "test.h"


static void device_init(void);

int main(void)
{     
    int a=0, b=995,c=0, var_d=0; 
    int toggle =0; 
    device_init();
    
    
    while(1)
    {
      
      
       AT91F_PIO_SetOutput(AT91C_BASE_PIOA,AT91C_PIO_PA18); 
       AT91F_PIO_SetOutput(AT91C_BASE_PIOA,AT91C_PIO_PA17);  
       
       AT91F_PIO_ClearOutput(AT91C_BASE_PIOA,AT91C_PIO_PA17); 
       AT91F_PIO_ClearOutput(AT91C_BASE_PIOA,AT91C_PIO_PA18); 
       
       AT91F_PIO_SetOutput(AT91C_BASE_PIOA,AT91C_PIO_PA17);        
       AT91F_PIO_SetOutput(AT91C_BASE_PIOA,AT91C_PIO_PA18); 
    
       AT91F_PIO_ClearOutput(AT91C_BASE_PIOA,AT91C_PIO_PA18); 
       AT91F_PIO_ClearOutput(AT91C_BASE_PIOA,AT91C_PIO_PA17); 
  
      
       a++;
       if(a==1000)
       {
         b++;
         a=0;

       }
       
       if(b==1000)
       {
          c++;
          b=0;
    
       }     
       
       var_d = add(a,b);
    }
    
}


static void device_init(void)
{

    /* When using the JTAG debugger the hardware is not always initialised to
	the correct default state.  This line just ensures that this does not
	cause all interrupts to be masked at the start. */
	
	AT91C_BASE_AIC->AIC_EOICR = 0;

    // Enable User Reset and set its minimal assertion to 960 us
    AT91C_BASE_RSTC->RSTC_RMR = AT91C_RSTC_URSTEN | (0x4<<8) | (unsigned int)(0xA5<<24);     
   
    init_dbgu(); // inits DBGU serial com
    
    TRACE_GLOBAL("- Global: Reset done\r\n");

    AT91F_PMC_EnablePeriphClock(AT91C_BASE_PMC,1<<AT91C_ID_PIOA); //enable PIO clock 4 Readout!
    AT91F_PIO_ClearOutput(AT91C_BASE_PIOA,AT91C_PIO_PA1);         //LED
    AT91F_PIO_SetOutput(AT91C_BASE_PIOA, AT91C_PIO_PA17 | AT91C_PIO_PA18);
    AT91F_PIO_CfgOutput(AT91C_BASE_PIOA,AT91C_PIO_PA1 | AT91C_PIO_PA17 | AT91C_PIO_PA18);

}







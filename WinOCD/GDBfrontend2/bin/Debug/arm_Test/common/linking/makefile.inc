OBJDUMP = arm-elf-objdump
OBJCOPY = arm-elf-objcopy

all-after:
	$(OBJCOPY) -O binary $(BIN) $(BIN).bin
	$(OBJDUMP) -h $(BIN) >> $(BIN).lss

clean-custom:
	${RM} $(BIN).bin
	${RM} $(BIN).lss

.PHONEY: all
.PHONEY: clean
PUML_FILES := $(shell find . -name "*.puml")
all: include.svg include.png

include.svg: $(PUML_FILES)
	plantuml -tsvg include.puml

%.png: %.svg
	magick $< -background transparent -trim $@

include.png: include.svg

clean:
	rm **.png{,.import}
conf <- read.table("average.txt", sep = "\n")

y = conf$V1
x = seq(1:length(y))

plot(x,y, type="b", bty="n", xaxt="n")
title(main="Average Function")

dev.new()
conf <- read.table("smoothaverage.txt", sep = "\n")

y = conf$V1
x = seq(1:length(y))

plot(x,y, type="b", bty="n", xaxt="n")
title(main="Smooth+Average Function")

dev.new()
conf <- read.table("progress.txt", sep = "\n")

y = conf$V1
x = seq(1:length(y))

plot(x,y, type="b", bty="n", xaxt="n")
title(main="Raw Plot")

dev.new()
conf <- read.table("progress.txt", sep = "\n")
conf <- read.table("smoothaverage.txt", sep = "\n")
y = conf$V1
x = seq(1:length(y))

y = fft(y)


plot(x,y, type="b", bty="n", xaxt="n")
title(main="FFT")
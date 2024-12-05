# Cirno
A simple programming language. **[WIP]**

## Hello, World! Program
```rust
"Hello, World!"
```

## Simple loop
```rust
i = 0
while i < 10
	print i + " "
	i = i + 1
end
# "0 1 2 3 4 5 6 7 8 9"
```

## Fibonacci Sequence
```rust
fn fib(n)
	a = 0
	b = 1
	while a < n
		print a + " "
		c = a
		a = b
		b = c + b
	end
end

fib(1000) # "0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 610 987"
```

## String replacement
```rust
x = "Goodbye, World!"
x["Goodbye"] = "Hello"
print x # "Hello, World!"
```

## 99 Bottles of Beer
```rust
fn println()
	print "\n"
end
fn println(v)
	print v
	print "\n"
end

i = 99

while true
	if i != 1
		println(i + " bottles of beer on the wall,")
		println(i + " bottles of beer.")
		println("Take one down, pass it around,")
		i = i - 1
		if i != 1
			println(i + " bottles of beer on the wall.")
		else
			println("1 bottle of beer on the wall.")
		end
	else
		println("1 bottle of beer on the wall,")
		println("1 bottle of beer.")
		println("Take one down, pass it around,")
		println("No bottles of beer on the wall.")

		break
	end

	println()
end
```
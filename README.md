# Cirno
A simple programming language. **[WIP]**

## Hello, World! Program
```rust
print("Hello, World!")
```

## Simple loop
```rust
for i in range(10)
	print(i, " ")
end
# "0 1 2 3 4 5 6 7 8 9"
```

## Hello program
```rust
while true
	out = input("What is your name? ")
	if out == ""
		break
	end
	print("Hello, " + out + "!")
end
```

## Fibonacci Sequence
```rust
fn fib(n)
	a = 0
	b = 1
	while a < n
		print(a, " ")
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
print(x) # "Hello, World!"
```

## 99 Bottles of Beer
```rust
i = 99

while true
	if i != 1
		print(i + " bottles of beer on the wall,")
		print(i + " bottles of beer.")
		print("Take one down, pass it around,")
		i = i - 1
		if i != 1
			print(i + " bottles of beer on the wall.")
		else
			print("1 bottle of beer on the wall.")
		end
	else
		print("1 bottle of beer on the wall,")
		print("1 bottle of beer.")
		print("Take one down, pass it around,")
		print("No bottles of beer on the wall.")

		break
	end

	print()
end
```

## Referencing other files & Global variables
`main.crn`
```rust
using "lib"

print(x) # 10, because it's global
print(y) # error, is not found
```
`other.crn`
```rust
global x
x = 10
y = 20
```

## typeof
```rust
a = ["h", 10, nova, true]
print(typeof a) # List
print(typeof a[0]) # "String"
print(typeof a[1]) # "Number"
print(typeof a[2]) # "Nova"
print(typeof a[3]) # "Bool"
```
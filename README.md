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
  print "" + i + "\n"
end
```

## Fibonacci Sequence
```rust
fn fib(n)
	a = 0
	b = 1
	while a < n
		print "" + a + " "
		c = a
		a = b
		b = c + b
	end
end

fib(1000)
# 0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 610 987
```

## String replacement
```rust
x = "Goodbye, World!"
x["Goodbye"] = "Hello"
print x # "Hello, World!"
```
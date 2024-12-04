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
	if n == 0
		return 0
	elif n == 1 or n == 2
		return 1
	else
		return fib(n - 1) + fib(n - 2)
	end
end

print fib(9)
```

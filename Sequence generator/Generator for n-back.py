import random

n = 2
hit_number = 5
block_length = 85

lower_bound = 3
upper_bound = 45

letters = list("ABCDEFGHIJKLMNOPQRSTUVWXYZ")

def generate_nback_sequence(n, block_length, hit_number, lower_bound, upper_bound):
    effective_lower = max(n, lower_bound)
    effective_upper = min(upper_bound, block_length - 1)


    possible_positions = list(range(effective_lower, effective_upper + 1))

    hit_positions = random.sample(possible_positions, hit_number)

    seq = [random.choice(letters) for _ in range(block_length)]

    for pos in hit_positions:
        seq[pos] = seq[pos - n]

    for pos in range(n, block_length):
        if pos not in hit_positions and seq[pos] == seq[pos - n]:
            choices = [L for L in letters if L != seq[pos - n]]
            seq[pos] = random.choice(choices)

    return seq


seq = generate_nback_sequence(n, block_length, hit_number, lower_bound, upper_bound)
print(seq)
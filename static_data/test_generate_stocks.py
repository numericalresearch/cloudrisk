from generate_stocks import *


def test_random_ticker():
    ticker = random_ticker(4, "W")
    assert ticker.endswith(".W")
    assert len(ticker) == 6

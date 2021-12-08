import random
import string
import json
import uuid


markets_and_currencies = {
    "GBP": ["L"],
    "EUR": ["F"],
    "USD": ["N"],
}


def random_ticker(ticker_len, suffix):
    return "".join(random.choices(string.ascii_uppercase, k=ticker_len)) + "." + suffix



def generate_stocks(nstocks, currency, ticker_len):
    stocks = []
    suffix = markets_and_currencies[currency][0]
    for _ in range(nstocks):
        ticker = random_ticker(ticker_len, suffix)
        stocks.append({
            "id": str(uuid.uuid4()),
            "ticker": ticker, 
            "currency": currency,
            # TODO 
        })
    return stocks






def main(argv):
    nstocks = 3
    ticker_len = 4

    stocks = generate_stocks(nstocks, "EUR", ticker_len)
    print(json.dumps(stocks, indent=4))
    


if __name__ == '__main__':
    import sys
    sys.exit(main(sys.argv))

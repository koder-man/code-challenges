#include <iostream>
#include <string>
#include <queue>
#include <tuple>
#include <vector>
#include <functional>
#include <algorithm>
#include <map>

using namespace std;
using str = string;

using u1 = uint8_t;
using u2 = uint16_t;
using u4 = uint32_t;
using u8 = uint64_t;
template<typename T>
using v = vector<T>;

using TT = u2;

#define R(TV, var) TV var; cin >> var;
#define RV(TV, var, cnt) v<TV> var; var.resize(cnt); for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) cin >> var[tmpCnt];
#define FOR(TV, var, cnt) for (TV var = 0; var < cnt; var++)

// https://codingcompetitions.withgoogle.com/kickstart/round/00000000001a0069
namespace N2020_G
{
  namespace N1KickStart
  {
    void ProcessCast1(TT i)
    {
      u8 cnt = 0;
      u4 ks = 0;

      u1 last = 0;
      string strCurrent;
      getline(cin, strCurrent);
      for (char c : strCurrent)
      {
        switch (c)
        {
        case 'K': // 1
          if (last == 3)
          {
            last = 1;
            ks++;
          }
          else
          {
            last = 1;
          }
          break;
        case 'I': // 2
          last = last == 1 ? 2 : 0;
          break;
        case 'C': // 3
          last = last == 2 ? 3 : 0;
          break;

        case 'S': // 5
          last = 5;
          break;
        case 'T': // 6
          if (last == 8)
          {
            last = 0;
            cnt += ks;
          }
          else
          {
            last = last == 5 ? 6 : 0;
          }
          break;
        case 'A': // 7
          last = last == 6 ? 7 : 0;
          break;
        case 'R': // 8
          last = last == 7 ? 8 : 0;
          break;
        default:
          last = 0;
          break;
        }
      }

      cout << "Case #" << i << ": " << cnt;
      cout << endl;
    }

    void ProcessCase(TT i)
    {
      u8 cnt = 0;
      u4 ks = 0;
      u1 id = 0;       // 012345678
      std::string pass = "KICKSTART";
      for (char c; cin.get(c) && c != '\n'; )
      {
        if (c != pass[id])
        {
          id = c == 'K' ? 1 : c == 'S' ? 5 : 0;
          continue;
        }

        switch (id)
        {
        case 3:
          id = 1;
          ks++;
          break;
        case 8:
          id = 0;
          cnt += ks;
          break;
        default:
          id++;
          break;
        }
      }

      cout << "Case #" << i << ": " << cnt;
      cout << endl;
    }
  }

  namespace N2MaxCoins
  {
    void ProcessCase(TT i)
    {
      using TN = u4;
      using TC = u4;
      using TCS = u8;

      TN N;
      cin >> N;

      vector<TCS> sumsUp;
      sumsUp.resize(N);
      vector<TCS> sumsLf;
      sumsLf.resize(N - 1);

      TC c;
      for (TN i = 0; i < N; i++)
      {
        for (TN j = 0; j < N; j++)
        {
          cin >> c;
          if (j >= i)
          {
            sumsUp[j - i] += c;
          }
          else
          {
            sumsLf[i - j - 1] += c;
          }
        }
      }
      TCS maxSum = 0;
      for (TCS i : sumsUp)
      {
        if (i > maxSum) maxSum = i;
      }
      for (TCS i : sumsLf)
      {
        if (i > maxSum) maxSum = i;
      }

      cout << "Case #" << i << ": " << maxSum;
      cout << endl;
    }
  }
}

namespace N2020_H
{
  namespace N1
  {

  }

  namespace N2
  {

  }

  namespace N3
  {

  }

  namespace N4
  {
    using TN = u2;
#define LEN(i, j) ((i) < (j) ? vvl[i][j - (i) - 1] : vvl[j][i - (j) - 1])

    bool AreFriends(const str& r1, const str& r2)
    {
      for (char c : r1)
      {
        if (r2.find(c) != str::npos)
        {
          return true;
        }
      }

      return false;
    }

    void ProcessCase(TT t)
    {
      R(TN, N);
      R(TN, Q);

      v<v<TN>> vvl;
      vvl.resize(N);
      RV(str, vs, N);

      FOR(TN, i, N)
      {
        vvl[i].resize(N - i - 1);
      }

      cout << "Case #" << t << ": ";
      do
      {
        R(TN, L);
        R(TN, R);

        L--;
        R--;
        TN& rl = LEN(L, R);
        if (!rl)
        {
          queue<TN> q;
          q.emplace(L);

          while (q.size())
          {
            TN cur = q.front();
            q.pop();

            for (TN i = 0; i < N; i++)
            {
              if (i == cur)
              {
                continue;
              }

              TN& rl1 = LEN(cur, i);
              if (rl1 == 0)
              {
                if (AreFriends(vs[cur], vs[i]))
                {
                  rl1 = 2;
                  continue;
                }
                q.emplace(rl1);
              }
            }
          }
        }

        if (rl == 1)
        {
          cout << -1;
        }
        else
        {
          cout << rl;
        }
      } while (--Q != 0);
    }
  }
}
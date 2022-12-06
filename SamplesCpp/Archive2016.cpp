#include <iostream>
#include <string>
#include <queue>
#include <tuple>
#include <vector>
#include <functional>
#include <algorithm>

using namespace std;
using str = string;

using u1 = uint8_t;
using u2 = uint16_t;
using u4 = uint32_t;
using u8 = uint64_t;

using TT = u2;
#define R(T, var) T var; cin >> var;
#define RV(T, var, cnt) v<T> var; var.resize(cnt); for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) cin >> var[tmpCnt];

// https://codingcompetitions.withgoogle.com/kickstart/round/0000000000201ca2
namespace N2016_A
{
  namespace N1CountryLeader
  {
    void ProcessCase(uint32_t i)
    {
      uint32_t n;
      cin >> n;

      string strLeader;
      uint32_t dwLeader = 0;

      string strCurrent;
      uint32_t dwCurrent = 0;

      const char cA = 'A';
      const char cZ = 'Z';

      for (uint32_t i = 0; i < n; i++)
      {
        std::cin >> std::ws;
        getline(cin, strCurrent);
        dwCurrent = 0;
        bool abIs[cZ - cA + 1] = { };

        for (char c : strCurrent)
        {
          if (c >= cA && c <= cZ && !abIs[c - cA])
          {
            dwCurrent++;
            abIs[c - cA] = 1;
          }
        }

        if (dwCurrent > dwLeader || (dwCurrent == dwLeader && strCurrent < strLeader))
        {
          strLeader = strCurrent;
          dwLeader = dwCurrent;
        }
      }
      cout << "Case #" << i << ": " << strLeader << endl;
    }
  }

  namespace N2Rain_NOT_FINISHED
  {

    using TR = u2;
    using TC = u2;
    using TSum = u4;

    using TH = u2;

    struct SH
    {
      TH m_H;
      TH m_HMax = -1;
      uint8_t m_byStatus = 0; // 0 start, 1 visited, 2 done

      inline bool CanVisit(TH h) { return !m_byStatus && m_HMax > h; }
      inline bool IsDone() { return m_byStatus == 2; }
      inline void Done() { m_byStatus = 2; m_HMax = m_H; }
    };
    struct SP
    {
      TR m_r;
      TC m_c;

      SP PU() { return SP{ static_cast<TR>(m_r - 1), static_cast<TC>(m_c + 0), }; };
      SP PR() { return SP{ static_cast<TR>(m_r - 0), static_cast<TC>(m_c + 1), }; };
      SP PD() { return SP{ static_cast<TR>(m_r + 1), static_cast<TC>(m_c - 0), }; };
      SP PL() { return SP{ static_cast<TR>(m_r + 0), static_cast<TC>(m_c - 1), }; };
    };

    void ProcessCase(TT i)
    {
      TR r;
      TC c;
      cin >> r >> c;

      vector<vector<SH>> mH;
      mH.resize(r);

      auto H = [&mH](SP Pt) -> SH& { return mH[Pt.m_r][Pt.m_c]; };

      for (TR i = 0; i < r; i++)
      {
        mH[i].resize(c);

        for (TC j = 0; j < c; j++)
        {
          cin >> mH[i][j].m_H;
        }
      }

      for (TR i = 1; i < r - 1; i++)
      {
        mH[i][0].Done();
        mH[i][c - 1].Done();
      }

      for (TC j = 1; j < c - 1; j++)
      {
        mH[0][j].Done();
        mH[r - 1][j].Done();
      }

      bool needLoop = true;
      while (needLoop)
      {
        needLoop = false;

        for (TR i = 1; i < r - 1; i++)
        {
          for (TC j = 1; j < c - 1; j++)
          {
            if (mH[i][j].IsDone())
            {
              continue;
            }

            queue<SP> v;
            v.push({ i, j, });
            mH[i][j].m_byStatus = 1;
            while (v.size())
            {
              SP p = v.front();
              v.pop();
              SH& rH = H(p);
              if (rH.IsDone())
              {
                continue;
              }
              rH.m_byStatus = 0;

              TH maxH = std::min({ H(p.PU()).m_HMax,
                 H(p.PL()).m_HMax, rH.m_HMax,        H(p.PR()).m_HMax,
                                   H(p.PD()).m_HMax, });

              if (maxH <= rH.m_H)
              {
                rH.Done();

                needLoop = true;
              }
              else if (maxH < rH.m_HMax)
              {
                rH.m_HMax = maxH;
                needLoop = true;
              }

              auto fpObserve = [&v, &rH, &H](SP pN)
              {
                if (H(pN).CanVisit(rH.m_HMax))
                {
                  v.push(pN);
                  H(pN).m_byStatus = 1;
                }
              };

              fpObserve(p.PU());
              fpObserve(p.PR());
              fpObserve(p.PD());
              fpObserve(p.PL());
            }
          }
        }
      }

      TSum totalH = 0;
      for (TR i = 1; i < r - 1; i++)
      {
        for (TC j = 1; j < c - 1; j++)
        {
          totalH += mH[i][j].m_HMax - mH[i][j].m_H;
        }
      }

      cout << "Case #" << i << ": " << totalH;
      cout << endl;
    }

    /*
5
3 3
3 5 5
5 4 5
5 5 5
4 4
5 5 5 1
5 1 1 5
5 1 5 5
5 2 5 8
4 3
2 2 2
2 1 2
2 1 2
2 1 2
5 11
0 9 8 7 6 5 4 3 2 3 4
9 0 1 2 3 4 3 2 1 2 4
9 0 1 2 3 4 3 2 1 2 4
9 0 1 2 3 4 3 2 1 2 4
0 9 8 7 6 5 4 3 2 3 4
7 11
0 9 8 7 6 5 4 3 2 3 4
9 0 1 2 3 6 3 2 1 2 4
9 6 5 4 3 5 3 2 1 2 4
9 0 1 2 3 6 3 2 1 2 4
9 3 4 5 6 7 3 2 1 2 4
9 0 1 2 3 4 3 2 1 2 4
0 9 8 7 6 5 4 3 2 3 4

10 * 3 + 2 + 5
    */
  }

  namespace N3FlowerShop
  {
#include <cmath>

    void ProcessCase(TT i)
    {
      using TM = u2;
      using TC = double;
      TM M;
      cin >> M;

      vector<TC> C;
      C.resize(M + 1);

      for (TM m = 0; m <= M; m++)
      {
        cin >> C[m];
      }
      C[0] = -C[0];

      double rMax = 1;
      double rMin = -1;
      double rOld = 10;
      double eps2 = 0.00000005;
      while (true)
      {
        double r = (rMax + rMin) / 2;

        if (r > rOld - eps2 && r < rOld + eps2)
        {
          break;
        }

        rOld = r;

        double r1 = 1;
        double total = 0;
        for (TM m = M; m <= M; m--)
        {
          total += C[m] * r1;
          r1 *= 1 + r;
        }

        if (total > 0)
        {
          rMin = r;
          continue;
        }
        if (total < 0)
        {
          rMax = r;
          continue;
        }
      }

      cout.precision(17);
      cout << "Case #" << i << ": " << rOld;
      cout << endl;
    }
  }

  namespace N4ClashRoyale
  {
    using TM = u4; // coins
    using TN = u2; // cards

    using TK = u2; // card level
    using TA = u4; // attack power
    using TC = u4; // cost to new level
    using TSum = u8; // total attack power

    struct SCard
    {
      TK m_Level;

      struct SPower
      {
        TA m_Power;
        TC m_UpCost;
      };
      vector<SPower> m_Powers;

      TA Power() { return m_Powers[m_Level].m_Power; }
      TC UpCost() { return m_Powers[m_Level].m_UpCost; }
      TK MaxL() { return static_cast<TK>(m_Powers.size()); }
    };

    void ProcessCase(TT i)
    {
      TM M; // coins
      TN N; // cards
      cin >> M >> N;

      vector<SCard> cards;
      cards.reserve(N);
      for (TN n = 0; n < N; n++)
      {
        TK levels;
        TK level;
        cin >> levels >> level;

        SCard Card;
        Card.m_Level = level - 1; // 1 based
        Card.m_Powers.resize(levels);

        for (TK l = 0; l < levels; l++)
        {
          cin >> Card.m_Powers[l].m_Power;
        }
        for (TK l = 0; l < levels - 1; l++)
        {
          cin >> Card.m_Powers[l].m_UpCost;
        }

        cards.push_back(Card);
      }

      vector<TK> lvls;
      lvls.resize(N);
      for (TN n = 0; n < N; n++)
      {
        lvls[n] = cards[n].m_Level;
      }

      TSum pMax = 0;
      TN pos = 0;
      TM remain = M;
      while (1)
      {
        SCard& rCard = cards[pos];
        if (lvls[pos] < rCard.MaxL() - 1 && rCard.m_Powers[lvls[pos]].m_UpCost <= remain)
        {
          remain -= rCard.m_Powers[lvls[pos]].m_UpCost;
          lvls[pos]++;

          TSum sum = 0;
          for (TN n = 0; n < N; n++)
          {
            sum += cards[n].m_Powers[lvls[n]].m_Power;
          }
          if (sum > pMax)
          {
            pMax = sum;
          }
          continue;
        }
        if (pos < N - 1)
        {
          pos++;
          continue;
        }

        bool breaked = false;
        for (TN n = N - 2; n < N; n--)
        {
          if (lvls[n] > cards[n].m_Level)
          {
            lvls[n] --;
            remain += cards[n].m_Powers[lvls[n]].m_UpCost;
            pos = n + 1;
            breaked = 1;
            break;
          }
        }

        if (breaked)
        {
          while (lvls[N - 1] > cards[N - 1].m_Level)
          {
            lvls[N - 1] --;
            remain += cards[N - 1].m_Powers[lvls[N - 1]].m_UpCost;
          }

          continue;
        }

        break;
      }

      cout << "Case #" << i << ": " << pMax;
      cout << endl;
    }
  }
}

namespace N2016_B
{
  namespace N1SherlockAndParentheses
  {
    void ProcessCase(TT i)
    {
      u8 L;
      u8 R;
      cin >> L >> R;
      u8 l = min(L, R);

      cout << "Case #" << i << ": " << (l * (l + 1) / 2);
      cout << endl;
    }
  }
}